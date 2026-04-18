namespace MathieuShop.Data
{
    public static class CurrentSession
    {
        public static User? CurrentUser { get; set; }

        public static bool IsAdmin =>
            CurrentUser?.Role?.Name == "Admin";
    }
}
