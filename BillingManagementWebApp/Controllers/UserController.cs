using AutoMapper;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Repositories;
using BillingManagementWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BillingManagementWebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly SmsLoggerService _smsLoggerService;
        public UserController(UserRepository userRepository, IMapper mapper, SmsLoggerService smsLoggerService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _smsLoggerService = smsLoggerService;
        }
        public async Task<IActionResult> Index()
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;
            if (role == "Admin")
            {
                var users = await _userRepository.GetAll();
                var userVms = _mapper.Map<List<UserViewModel>>(users);
                return View(userVms);
            }
            else
            {
                var user = await _userRepository.GetByEmail(User.FindFirst(ClaimTypes.Email).Value);
                var userVm = _mapper.Map<UserViewModel>(user);
                return View("Details",userVm);
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var user = await _userRepository.GetById(id.Value);
                var userVm = _mapper.Map<UserViewModel>(user);
                return View(userVm);
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
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel vm)
        {
            if(vm==null) 
                throw new ArgumentNullException(nameof(vm));
            var user = _mapper.Map<User>(vm);
            if (user == null)
                throw new InvalidOperationException("cannot be mapped!");
            user.Password = Guid.NewGuid().ToString().Substring(0, 8);
            await _userRepository.Create(user);
            _smsLoggerService.Write("Welcome to your new home, we have created an account for you to track your expenses an such and this is your password to log in: "+user.Password+". You can change your password from the system. All the apartment related issues will be handled here! Have a great day!");
            return Ok();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
                throw new ArgumentNullException(nameof(id));
            var user = await _userRepository.GetById(id.Value);
            if(user == null)
                throw new FileNotFoundException(nameof(user));
            var userVm = _mapper.Map<CreateUserViewModel>(user);
            if (userVm == null)
                throw new InvalidOperationException("cannot be mapped!");
            return View(userVm);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody]CreateUserViewModel vm)
        {
            if(id==null||vm==null)
                throw new ArgumentNullException(nameof(id),nameof(vm));
            var user = _mapper.Map<User>(vm);
            if (user == null)
                throw new FileNotFoundException(nameof(user));
            if(user.Password==null)
                user.Password = _userRepository.GetByIdNoTracking(id).Result.Password;
            await _userRepository.Update(user);
            return Ok();
        }
        public async Task<IActionResult> Delete(int?id)

        {
            if(id==null)
                throw new ArgumentNullException(nameof(id));
            var user = await _userRepository.GetById(id.Value);
            if (user == null)
                throw new FileNotFoundException(nameof(user));  
            return View(_mapper.Map<UserViewModel>(user));
        }
        [HttpDelete,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id==null)
                throw new ArgumentNullException(nameof(id));
            var user = await _userRepository.GetById(id.Value);
            if (user == null)
                throw new FileNotFoundException(nameof(user));
            await _userRepository.Delete(user);
            return Ok();
        }
    }
}
