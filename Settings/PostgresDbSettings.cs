namespace TwitterAPI.Settings {
	class PostgresDbSettings {
		public static string DB_NAME = "twitter-api";
		public string Host { get; set; }
		public int Port { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Database { get; set; }
		public string ConnectionString {
			get {
				return $"Host={Host};Port={Port};Username={User};Password={Password};Database={Database}";
			}
		}
	}
}