namespace TwitterAPI.Settings {
	class PostgresDbSettings {
		public static string DB_NAME = "twitter-api";
		public string Host { get; set; } = null!;
		public int Port { get; set; }
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string Database { get; set; } = null!;
		public string ConnectionString {
			get {
				return $"Host={Host};Port={Port};Username={User};Password={Password};Database={Database}";
			}
		}
	}
}