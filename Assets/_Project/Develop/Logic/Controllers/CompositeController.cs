namespace _Project.Develop.Logic.Controllers
{
    public class CompositeController : Controller
    {
        private Controller[] _controllers;

        public CompositeController(params Controller[] controllers)
        {
            _controllers = controllers;
        }

        protected override void UpdateLogic(float deltaTime)
        {
            foreach (Controller controller in _controllers)
                controller.Update(deltaTime);
        }

        public override void Enable()
        {
            base.Enable();

            foreach (Controller controller in _controllers)
                controller.Enable();
        }

        public override void Disable()
        {
            base.Disable();

            foreach (Controller controller in _controllers)
                controller.Disable();
        }
    }
}