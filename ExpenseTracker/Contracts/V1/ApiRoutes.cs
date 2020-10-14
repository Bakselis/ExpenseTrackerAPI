namespace ExpenseTracker.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Expenses
        {
            public const string GetAll = Base + "/expenses";
            public const string Update = Base + "/expenses/{expenseId}";
            public const string Delete = Base + "/expenses/{expenseId}";
            public const string Get = Base + "/expenses/{expenseId}";
            public const string Create = Base + "/expenses";
        }
        
        public static class Tags
        {
            public const string GetAll = Base + "/tags";
            public const string Get = Base + "/tags/{tagName}";
            public const string Create = Base + "/tags";
            public const string Delete = Base + "/tags/{tagName}";
        }
    }
}