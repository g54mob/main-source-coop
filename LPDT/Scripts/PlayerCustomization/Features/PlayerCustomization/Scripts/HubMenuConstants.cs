using System.Collections.Generic;

namespace Features.PlayerCustomization.Scripts
{
	public static class HubMenuConstants
	{
		public static readonly string PossibleNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890_ ";

		public static readonly string PossibleCodeCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

		public static readonly IReadOnlyList<string> DummyNames = new List<string>
		{
			"Mac Mac", "Johnny", "Burgerito", "Taco Queen", "Emily", "Hot dog", "Lemon Pie", "Ashley", "Cinnamon", "Bobby",
			"Pancake", "Sophie", "Nugget", "Chips", "Katie", "Waffle", "Spaghetti", "Grace", "Pizzaz", "Lily",
			"Dave", "Ben", "Salvadora", "Ravioli", "Jack", "Apple", "Buffalo", "Fred", "Salsa Sam", "Cherry Pie",
			"Noodle", "Cheesecake", "Sandy", "Greg", "Toast", "Muffin", "Oscar", "Savannah", "Pickle", "Benny",
			"Coco", "Carl", "Mango", "Cupcake", "Larry", "Brody", "Gabe", "Pudding", "Ethan", "Beth",
			"Bacon", "Marshmallow", "Tuna Tina", "Nacho Nick", "Zach", "Coleslaw", "Coby", "Peach", "Chris", "Logan",
			"Brownie", "Terry", "Chad", "Fries Fran", "Bobby", "Sugar", "Waffles", "Sloppy", "Casserole", "Eggs Ella",
			"Cookie", "Dylan", "Jason", "Sally", "Jack", "Matt", "Steve", "Tyler", "Toastie", "Salmon",
			"Ice cream", "Randy", "Taffy", "Sausage", "George", "Nina", "Cornbread", "Casey", "Quiche", "Donut",
			"Pasta", "Snacks", "Harper", "Brenda", "Meatball", "Coconut", "Roxy", "Zoe", "Mason"
		};
	}
}
