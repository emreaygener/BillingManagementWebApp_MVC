using AutoMapper;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BillingManagementWebApp.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;
        public InvoiceController(InvoiceRepository invoiceRepository, IMapper mapper, UserRepository userRepository)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            if (User.FindFirst(ClaimTypes.Role).Value != "Admin")
            {
                var invoicesForNonAdmin = await _invoiceRepository.GetAllForNonAdmin(User.FindFirst(ClaimTypes.Email).Value);
                var invoiceVmsForNonAdmin = _mapper.Map<List<InvoiceViewModel>>(invoicesForNonAdmin);
                return View(invoiceVmsForNonAdmin);
            }
            else
            {
                var invoices = await _invoiceRepository.GetAll();
                var invoiceVms = _mapper.Map<List<InvoiceViewModel>>(invoices);
                return View(invoiceVms);
            }
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvoiceViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var invoice = _mapper.Map<Invoice>(vm);
            if (invoice == null)
                throw new InvalidOperationException("cannot be mapped!");
            var user = await _userRepository.GetByTc(Convert.ToInt64(vm.User));
            invoice.User = user;
            invoice.UserId = user.Id;
            await _invoiceRepository.Create(invoice);
            return Ok();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var invoice = await _invoiceRepository.GetById(id.Value);
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
            if (vm.DateCreated == null)
            {
                vm.DateCreated = _invoiceRepository.GetByIdAsNoTracking(id.Value).Result.DateCreated;
            }
            var invoice = _mapper.Map<Invoice>(vm);
            if (invoice == null)
                throw new FileNotFoundException(nameof(invoice));
            var user = await _userRepository.GetByTc(Convert.ToInt64(vm.User));
            invoice.User = user;
            invoice.UserId = user.Id;

            await _invoiceRepository.Update(invoice);
            return Ok();
        }
        public async Task<IActionResult> Delete(int? id)

        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var invoice = await _invoiceRepository.GetById(id.Value);
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
            return Ok();
        }
        [HttpPost,ActionName("CreateInvoiceForAllUsers")]
        public async Task<IActionResult> CreateInvoiceForAllUsers([FromBody]InvoiceViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var users = await _userRepository.GetAll();
            foreach (var user in users)
            {
                var invoice = _mapper.Map<Invoice>(vm);
                if (invoice == null)
                    throw new InvalidOperationException("cannot be mapped!");
                invoice.User = user;
                invoice.UserId = user.Id;
                await _invoiceRepository.Create(invoice);
            }
            return Ok();
        }
    }
}
