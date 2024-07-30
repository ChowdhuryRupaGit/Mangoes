namespace WebAppMango.Utilities
{
    public static class SD
    {
        public static string CouponBaseUrl { get; set; }
        public static string AuthAPIUrl { get; set; }
        public static string ProductAPIUrl { get; set; }
        public static string CartAPIUrl { get; set; }
        public static string OrderAPIUrl { get; set; }

        public static string RoleAdmin = "Admin";
        public static string RoleCustomer = "Customer";
        public static string Token = "";

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_Completed = "Completed";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";
        public const string Status_ReadyForPickUp = "Ready For PickUp";
        public enum APITypes
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
