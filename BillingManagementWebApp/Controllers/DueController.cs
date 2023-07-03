using AutoMapper;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingManagementWebApp.Controllers
{
    [Authorize]
    public class DueController : Controller
    {
        private readonly DueRepository _dueRepository;
        private readonly IMapper _mapper;
        public DueController(DueRepository dueRepository, IMapper mapper)
        {
            _dueRepository = dueRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var dues = await _dueRepository.GetAll();
            var dueVms = _mapper.Map<List<DueViewModel>>(dues);
            return View(dueVms);
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var due = await _dueRepository.GetById(id.Value);
                var dueVm = _mapper.Map<DueViewModel>(due);
                return View(dueVm);
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

        public async Task<IActionResult> Create([FromBody] DueViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var due = _mapper.Map<Due>(vm);
            if (due == null)
                throw new InvalidOperationException("cannot be mapped!");
            await _dueRepository.Create(due);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var due = _dueRepository.GetById(id.Value);
            if (due == null)
                throw new FileNotFoundException(nameof(due));
            var dueVm = _mapper.Map<DueViewModel>(due);
            if (dueVm == null)
                throw new InvalidOperationException("cannot be mapped!");
            return View(dueVm);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(int? id, [FromBody] DueViewModel vm)
        {
            if (id == null || vm == null)
                throw new ArgumentNullException(nameof(id), nameof(vm));
            var due = _mapper.Map<Due>(vm);
            if (due == null)
                throw new FileNotFoundException(nameof(due));

            await _dueRepository.Update(due);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)

        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var due = _dueRepository.GetById(id.Value);
            if (due == null)
                throw new FileNotFoundException(nameof(due));
            return View(_mapper.Map<DueViewModel>(due));
        }
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var due = await _dueRepository.GetById(id.Value);
            if (due == null)
                throw new FileNotFoundException(nameof(due));
            await _dueRepository.Delete(due);
            return RedirectToAction("Index");
        }
    }
}
