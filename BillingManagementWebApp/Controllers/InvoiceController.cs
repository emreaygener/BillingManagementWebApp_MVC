using AutoMapper;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingManagementWebApp.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        public InvoiceController(InvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceRepository.GetAll();
            var invoiceVms = _mapper.Map<List<InvoiceViewModel>>(invoices);
            return View(invoiceVms);
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var invoice = await _invoiceRepository.GetById(id.Value);
                var invoiceVm = _mapper.Map<InvoiceViewModel>(invoice);
                return View(invoiceVm);
            }
            catch
            {
                throw new Exception();
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create([FromBody] InvoiceViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var invoice = _mapper.Map<Invoice>(vm);
            if (invoice == null)
                throw new InvalidOperationException("cannot be mapped!");
            await _invoiceRepository.Create(invoice);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var invoice = _invoiceRepository.GetById(id.Value);
            if (invoice == null)
                throw new FileNotFoundException(nameof(invoice));
            var invoiceVm = _mapper.Map<InvoiceViewModel>(invoice);
            if (invoiceVm == null)
                throw new InvalidOperationException("cannot be mapped!");
            return View(invoiceVm);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(int? id, [FromBody] InvoiceViewModel vm)
        {
            if (id == null || vm == null)
                throw new ArgumentNullException(nameof(id), nameof(vm));
            var invoice = _mapper.Map<Invoice>(vm);
            if (invoice == null)
                throw new FileNotFoundException(nameof(invoice));

            await _invoiceRepository.Update(invoice);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)

        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var invoice = _invoiceRepository.GetById(id.Value);
            if (invoice == null)
                throw new FileNotFoundException(nameof(invoice));
            return View(_mapper.Map<InvoiceViewModel>(invoice));
        }
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var invoice = await _invoiceRepository.GetById(id.Value);
            if (invoice == null)
                throw new FileNotFoundException(nameof(invoice));
            await _invoiceRepository.Delete(invoice);
            return RedirectToAction("Index");
        }
    }
}
