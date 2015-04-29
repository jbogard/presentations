namespace ContosoUniversity.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using MediatR;
    using Features.Student;

    public class StudentController : Controller
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /Student/
        public ViewResult Index(Index.Query query)
        {
            var model = _mediator.Send(query);

            return View(model);
        }

        // GET: /Student/Details/5
        public async Task<ActionResult> Details(Details.Query query)
        {
            var student = await _mediator.SendAsync(query);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Create.Command model)
        {
            _mediator.Send(model);

            return this.RedirectToActionJson("Index");
        }


        // GET: /Student/Edit/5
        public async Task<ActionResult> Edit(Edit.Query query)
        {
            var student = await _mediator.SendAsync(query);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Edit.Command model)
        {
            await _mediator.SendAsync(model);

            return this.RedirectToActionJson("Index");
        }

        // GET: /Student/Delete/5
        public async Task<ActionResult> Delete(Delete.Query query)
        {
            var student = await _mediator.SendAsync(query);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Delete.Command model)
        {
            await _mediator.SendAsync(model);

            return this.RedirectToActionJson("Index");
        }
    }
}
