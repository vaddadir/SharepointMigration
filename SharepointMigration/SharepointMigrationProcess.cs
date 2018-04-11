namespace SharepointMigration
{
    public class SharepointMigrationProcess
    {
        //Create source context
        private Context sourceContext = new Context(AppSettingsProvider.Instance.SourceServerUrl, AppSettingsProvider.Instance.SourceServerCredentials);

        //Create target context
        private Context targetContext = new Context(AppSettingsProvider.Instance.TargetServerUrl, AppSettingsProvider.Instance.TargetServerCredentials);

        public void Run()
        {
            //ImportGroups();
            ImportLists();
        }

        private void ImportGroups()
        {
            GroupImportContext groupImportContext = new GroupImportContext(sourceContext, targetContext);

            GroupImporter groupImporter = new GroupImporter();
            groupImporter.Import(groupImportContext);
        }

        private void ImportLists()
        {
            //Create and setup list import context
            ListImportContext listImportContext = new ListImportContext(sourceContext, targetContext);
            listImportContext.ListNames.AddRange(AppSettingsProvider.Instance.GetListNamesToImport());

            //Import lists
            ListImporter listImporter = new ListImporter();
            listImporter.Import(listImportContext);

            //Verify imported lists
            listImporter.Verify(listImportContext);
        }
    }
}