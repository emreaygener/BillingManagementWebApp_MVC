using AutoMapper;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BillingManagementWebApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly MessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessageController(MessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var messages = await _messageRepository.GetAll();
            var messageVms = _mapper.Map<List<MessageViewModel>>(messages);
            return View(messageVms);
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var message = await _messageRepository.GetById(id.Value);
                var messageVm = _mapper.Map<MessageViewModel>(message);
                return View(messageVm);
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

        public async Task<IActionResult> Create([FromBody] MessageViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var message = _mapper.Map<Message>(vm);
            if (message == null)
                throw new InvalidOperationException("cannot be mapped!");
            await _messageRepository.Create(message);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var message = _messageRepository.GetById(id.Value);
            if (message == null)
                throw new FileNotFoundException(nameof(message));
            var messageVm = _mapper.Map<MessageViewModel>(message);
            if (messageVm == null)
                throw new InvalidOperationException("cannot be mapped!");
            return View(messageVm);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(int? id, [FromBody] MessageViewModel vm)
        {
            if (id == null || vm == null)
                throw new ArgumentNullException(nameof(id), nameof(vm));
            var message = _mapper.Map<Message>(vm);
            if (message == null)
                throw new FileNotFoundException(nameof(message));

            await _messageRepository.Update(message);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)

        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var message = _messageRepository.GetById(id.Value);
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
            return RedirectToAction("Index");
        }
    }
}
