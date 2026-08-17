namespace VContainer.Internal
{
	internal struct RegistrationElement
	{
		public Registration Registration;

		public IObjectResolver RegisteredContainer;

		public RegistrationElement(Registration registration, IObjectResolver registeredContainer)
		{
			Registration = registration;
			RegisteredContainer = registeredContainer;
		}
	}
}
