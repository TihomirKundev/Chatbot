import com.structurizr.Workspace;
import lombok.extern.slf4j.Slf4j;
import utils.DirectoryCleaner;
import utils.WorkspaceUploader;
import workspaces.ChatBot;

import java.awt.*;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;

@Slf4j
public class Main
{
    private static final String structurizrUrl = "https://structurizr.com/share/";

    public static void main(String[] args) throws Exception
    {
        Workspace workspace = new ChatBot().getWorkspace();

        WorkspaceUploader.Upload(workspace);

        DirectoryCleaner.Clean();

        OpenDiagramInBrowser();
    }

    private static void OpenDiagramInBrowser()
    {
        String id = System.getenv().get("STRUCTURIZR_WORKSPACE_ID").strip();
        String urlString = structurizrUrl + id + "/diagrams#";
        try{
            var uri = new URI(urlString);
            if (Desktop.isDesktopSupported() && Desktop.getDesktop().isSupported(Desktop.Action.BROWSE))
            {
                Desktop.getDesktop().browse(uri);
                return;
            }
        } catch (URISyntaxException e) {
            log.error("Invalid URI format: " + urlString);
        } catch (IOException e) {
            log.error("Failed to open browser.");
        }
        log.info("The diagram can be accessed in your browser with URL: " + urlString);
    }
}
