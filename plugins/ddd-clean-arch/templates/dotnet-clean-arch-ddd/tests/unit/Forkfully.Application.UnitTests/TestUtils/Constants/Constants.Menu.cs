namespace Forkfully.Application.UnitTests.TestUtils;

public static partial class Constants
{
    public static class Menu
    {
        public const string Name = "Menu Name";
        public const string Description = "Menu Description";

        public const string MenuSectionName = "Menu Section Name";
        public const string MenuSectionDescription = "Menu Section Description";

        public const string MenuItemName = "Menu Item Name";
        public const string MenuItemDescription = "Menu Item Description";

        // Per-index variants keep each generated section/item unique.
        public static string SectionNameFromIndex(int index) => $"{MenuSectionName} {index}";
        public static string SectionDescriptionFromIndex(int index) => $"{MenuSectionDescription} {index}";
        public static string ItemNameFromIndex(int index) => $"{MenuItemName} {index}";
        public static string ItemDescriptionFromIndex(int index) => $"{MenuItemDescription} {index}";
    }
}
