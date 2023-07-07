using AutoMapper;
using BillingManagementWebApp.Data;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BillingManagementWebApp.Controllers
{
    [Authorize]
    public class FlatController:Controller
    {
        private readonly FlatRepository _flatRepository;
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;
        public FlatController(FlatRepository flatRepository, IMapper mapper, UserRepository userRepository)
        {
            _flatRepository = flatRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            if (User.FindFirst(ClaimTypes.Role).Value != "Admin")
            {
                var flatsForNonAdmin = await _flatRepository.GetAllForNonAdmin(User.FindFirst(ClaimTypes.Email).Value);
                var flatVmsForNonAdmin = _mapper.Map<List<FlatViewModel>>(flatsForNonAdmin);
                return View(flatVmsForNonAdmin);
            }
            else {
                var flats = await _flatRepository.GetAll();
                var flatVms = _mapper.Map<List<FlatViewModel>>(flats);
                return View(flatVms);
            }
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FlatViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var user= await _userRepository.GetByTc(Convert.ToInt64(vm.User));
            var flat = _mapper.Map<Flat>(vm);
            if (flat == null)
                throw new InvalidOperationException("cannot be mapped!");
            flat.User = user;
            flat.UserId = user.Id;
            await _flatRepository.Create(flat);
            return Ok();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var flat = await _flatRepository.GetById(id.Value);
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
            var user = await _userRepository.GetByTc(Convert.ToInt64(vm.UserTc));
            flat.User = user;
            flat.UserId = user.Id;

            await _flatRepository.Update(flat);
            return Ok();
        }
        public async Task<IActionResult> Delete(int? id)

        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var flat = await _flatRepository.GetById(id.Value);
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
            return Ok();
        }

    }
}
