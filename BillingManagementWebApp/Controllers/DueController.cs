﻿using AutoMapper;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BillingManagementWebApp.Controllers
{
    [Authorize]
    public class DueController : Controller
    {
        private readonly DueRepository _dueRepository;
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;
        public DueController(DueRepository dueRepository, IMapper mapper, UserRepository userRepository)
        {
            _dueRepository = dueRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            if (User.FindFirst(ClaimTypes.Role).Value != "Admin")
            { 
                var duesForNonAdmin = await _dueRepository.GetAllForNonAdmin(User.FindFirst(ClaimTypes.Email).Value);
                var dueVmsForNonAdmin = _mapper.Map<List<DueViewModel>>(duesForNonAdmin);
                return View(dueVmsForNonAdmin);
            }
            else
            {
                var dues = await _dueRepository.GetAll();
                var dueVms = _mapper.Map<List<DueViewModel>>(dues);
                return View(dueVms);

            }
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DueViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var due = _mapper.Map<Due>(vm);
            if (due == null)
                throw new InvalidOperationException("cannot be mapped!");
            var user = await _userRepository.GetByTc(Convert.ToInt64(vm.User));
            due.User = user;
            due.UserId = user.Id;
            await _dueRepository.Create(due);
            return Ok();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var due = await _dueRepository.GetById(id.Value);
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
            var user = await _userRepository.GetByTc(Convert.ToInt64(vm.UserTc));
            due.User = user;
            due.UserId = user.Id;

            await _dueRepository.Update(due);
            return Ok();
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
            return Ok();
        }
    }
}
