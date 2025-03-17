using C__and_Project.Models;
using C__and_Project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace C__and_Project.Controllers
{
    public class RoomController: Controller
    {
        private readonly IRoomRepository _roomRepository;

        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IActionResult Index()
        {
            var rooms = _roomRepository.GetAllRooms();
            return View(rooms);
        }

        public IActionResult Details(int id)
        {
            var room = _roomRepository.GetRoomById(id);
            if (room == null)
                return NotFound();
            return View(room);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                _roomRepository.AddRoom(room);
                return RedirectToAction("Index");
            }
            return View(room);
        }
    }
}
