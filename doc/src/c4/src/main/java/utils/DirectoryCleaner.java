package utils;

import lombok.extern.slf4j.Slf4j;

import java.io.File;
import java.util.Set;

@Slf4j
public class DirectoryCleaner
{
    private static final File sourcePath = new File(System.getProperty("user.dir"));
    private static final Set<String> targetExtensions = Set.of(".json");

    public static void Clean()
    {
        log.info("Cleaning directory: " + sourcePath);
        var files =  sourcePath.listFiles();
        if(files == null)
        {
            log.error("No files found in " + sourcePath.getAbsolutePath());
            return;
        }
        for(File file : files)
        {
            for (String extension : targetExtensions)
            {
                if(file.getName().endsWith(extension))
                {
                    if(!file.delete())
                        log.error("Failed to delete " + file.getAbsolutePath());
                }

            }
        }
    }

    private DirectoryCleaner() {}
}
