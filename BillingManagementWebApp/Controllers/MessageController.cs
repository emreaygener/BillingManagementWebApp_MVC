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
    public class MessageController : Controller
    {
        private readonly MessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;
        public MessageController(MessageRepository messageRepository, IMapper mapper, UserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var messages = await _messageRepository.GetAllReceived(User.FindFirst(ClaimTypes.Email).Value);
            var messageVms = _mapper.Map<List<MessageViewModel>>(messages);
            return View(messageVms);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var message = _mapper.Map<Message>(vm);
            if (message == null)
                throw new InvalidOperationException("cannot be mapped!");
            var sender = await _userRepository.GetByEmail(User.FindFirst(ClaimTypes.Email).Value);
            var receiver = await _userRepository.GetByCredentials(vm.ReceiverUsername);
            if (sender == null || receiver == null)
                throw new FileNotFoundException(nameof(sender), nameof(receiver));
            message.SenderId = sender.Id;
            message.ReceiverId = receiver.Id;
            message.Sender = sender;
            message.Receiver = receiver;
            message.DateSent = DateTime.Now;
            await _messageRepository.Create(message);
            return Ok();
        }
        public async Task<IActionResult> Delete(int? id)

        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var message = await _messageRepository.GetById(id.Value);
            if (message == null)
                throw new FileNotFoundException(nameof(message));
            return View(_mapper.Map<MessageViewModel>(message));
        }
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var message = await _messageRepository.GetById(id.Value);
            if (message == null)
                throw new FileNotFoundException(nameof(message));
            await _messageRepository.Delete(message);
            return Ok();
        }
        [HttpPost,ActionName("SendMessageToAllUsers")]
        public async Task<IActionResult> SendMessageToAllUsers([FromBody] MessageViewModel vm)
        {
            var users = await _userRepository.GetAll();
            foreach (var user in users)
            {
                var message = new Message();
                message.SenderId = (await _userRepository.GetByEmail(User.FindFirst(ClaimTypes.Email).Value)).Id;
                message.DateSent = DateTime.Now;
                message.Sender = await _userRepository.GetByEmail(User.FindFirst(ClaimTypes.Email).Value);
                message.Header = vm.Header;
                message.Content = vm.Content;
                message.ReceiverId = user.Id;
                message.Receiver = user;
                await _messageRepository.Create(message);
            }
            return Ok();
        }
        public async Task<IActionResult> SentMessages()
        {
            var messages = await _messageRepository.GetAllSent(User.FindFirst(ClaimTypes.Email).Value);
            var messageVms = _mapper.Map<List<MessageViewModel>>(messages);
            return View(messageVms);
        }
    }
}
