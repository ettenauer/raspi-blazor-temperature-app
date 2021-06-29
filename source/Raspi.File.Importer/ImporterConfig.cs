namespace Raspi.File.Importer
{
    internal class ImporterConfig
    {
        public string StorageAccountName { get; set; }

        public string StorageAccountKey { get; set; }

        public string IdentityClientId { get; set; }

        public string IdentityClientSecret { get; set; }

        public string IdentityAuthority { get; set; }

        public string ImportApiUri { get; set; }

        public bool AllSet => !string.IsNullOrEmpty(StorageAccountKey) &&
            !string.IsNullOrEmpty(StorageAccountName) &&
            !string.IsNullOrEmpty(IdentityClientId) &&
            !string.IsNullOrEmpty(IdentityClientSecret) &&
            !string.IsNullOrEmpty(IdentityAuthority) &&
            !string.IsNullOrEmpty(ImportApiUri);
    }
}
