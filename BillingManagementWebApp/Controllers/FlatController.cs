using AutoMapper;
using BillingManagementWebApp.Data;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BillingManagementWebApp.Controllers
{
    public class FlatController:Controller
    {
        private readonly FlatRepository _flatRepository;
        private readonly IMapper _mapper;
        public FlatController(FlatRepository flatRepository, IMapper mapper)
        {
            _flatRepository = flatRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var flats = await _flatRepository.GetAll();
            var flatVms = _mapper.Map<List<FlatViewModel>>(flats);
            return View(flatVms);
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var flat = await _flatRepository.GetById(id.Value);
                var flatVm = _mapper.Map<FlatViewModel>(flat);
                return View(flatVm);
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

        public async Task<IActionResult> Create([FromBody] FlatViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var flat = _mapper.Map<Flat>(vm);
            if (flat == null)
                throw new InvalidOperationException("cannot be mapped!");
            await _flatRepository.Create(flat);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var flat = _flatRepository.GetById(id.Value);
            if (flat == null)
                throw new FileNotFoundException(nameof(flat));
            var flatVm = _mapper.Map<FlatViewModel>(flat);
            if (flatVm == null)
                throw new InvalidOperationException("cannot be mapped!");
            return View(flatVm);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(int? id, [FromBody] FlatViewModel vm)
        {
            if (id == null || vm == null)
                throw new ArgumentNullException(nameof(id), nameof(vm));
            var flat = _mapper.Map<Flat>(vm);
            if (flat == null)
                throw new FileNotFoundException(nameof(flat));

            await _flatRepository.Update(flat);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)

        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var flat = _flatRepository.GetById(id.Value);
            if (flat == null)
                throw new FileNotFoundException(nameof(flat));
            return View(_mapper.Map<FlatViewModel>(flat));
        }
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var flat = await _flatRepository.GetById(id.Value);
            if (flat == null)
                throw new FileNotFoundException(nameof(flat));
            await _flatRepository.Delete(flat);
            return RedirectToAction("Index");
        }

    }
}
