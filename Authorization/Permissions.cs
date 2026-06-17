namespace Feline_Gallery_v1.Authorization
{
    public static class Permissions
    {
        // Artwork Permissions
        public const string ViewArtworks = "Permissions.Artworks.View";
        public const string CreateArtworks = "Permissions.Artworks.Create";
        public const string EditArtworks = "Permissions.Artworks.Edit";
        public const string DeleteArtworks = "Permissions.Artworks.Delete";

        // Category Permissions
        public const string ViewCategories = "Permissions.Categories.View";
        public const string CreateCategories = "Permissions.Categories.Create";
        public const string EditCategories = "Permissions.Categories.Edit";
        public const string DeleteCategories = "Permissions.Categories.Delete";

        // Artist Permissions
        public const string ViewArtists = "Permissions.Artists.View";
        public const string CreateArtists = "Permissions.Artists.Create";
        public const string EditArtists = "Permissions.Artists.Edit";
        public const string DeleteArtists = "Permissions.Artists.Delete";

        // Exhibition Permissions
        public const string ViewExhibitions = "Permissions.Exhibitions.View";
        public const string CreateExhibitions = "Permissions.Exhibitions.Create";
        public const string EditExhibitions = "Permissions.Exhibitions.Edit";
        public const string DeleteExhibitions = "Permissions.Exhibitions.Delete";

        // Order Permissions
        public const string ViewOrders = "Permissions.Orders.View";
        public const string ViewOwnOrders = "Permissions.Orders.ViewOwn";
        public const string ManageOrders = "Permissions.Orders.Manage";

        // Admin Dashboard
        public const string AccessAdminDashboard = "Permissions.Admin.Dashboard";
    }
}