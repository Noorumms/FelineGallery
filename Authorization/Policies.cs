namespace Feline_Gallery_v1.Authorization
{
    public static class Policies
    {
        // Admin Policies
        public const string RequireAdminAccess = "RequireAdminAccess";
        public const string RequireArtworkManagement = "RequireArtworkManagement";
        public const string RequireCategoryManagement = "RequireCategoryManagement";
        public const string RequireArtistManagement = "RequireArtistManagement";
        public const string RequireExhibitionManagement = "RequireExhibitionManagement";
        public const string RequireOrderManagement = "RequireOrderManagement";

        // Customer Policies
        public const string RequireCustomerAccess = "RequireCustomerAccess";
        public const string RequireOrderView = "RequireOrderView";
    }
}
