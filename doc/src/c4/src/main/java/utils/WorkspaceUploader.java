package utils;

import com.structurizr.Workspace;
import com.structurizr.api.StructurizrClient;
import com.structurizr.api.StructurizrClientException;
import lombok.extern.slf4j.Slf4j;

import java.awt.*;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;

@Slf4j
public class WorkspaceUploader
{
    private static final String apiKey_label = "STRUCTURIZR_API_KEY";
    private static final String apiSecret_label = "STRUCTURIZR_API_SECRET";
    private static final String workspaceId_label = "STRUCTURIZR_WORKSPACE_ID";

    public static boolean Upload(Workspace workspace)
    {
        var apiKey = System.getenv().get(apiKey_label);
        var secret = System.getenv().get(apiSecret_label);
        var workspaceId_str = System.getenv().get(workspaceId_label);

        if(apiKey == null || secret == null || workspaceId_str == null)
        {
            String message = "Please set the environment variables:";
            if(apiKey == null)
                message += "\n\t" + apiKey_label;
            if(secret == null)
                message += "\n\t" + apiSecret_label;
            if(workspaceId_str == null)
                message += "\n\t" + workspaceId_label;
            log.error(message);
            return false;
        }

        long workspaceId;
        try{
            workspaceId = Long.parseLong(workspaceId_str.strip());
        } catch (NumberFormatException e) {
            log.error("Please set the environment variable STRUCTURIZR_WORKSPACE_ID to a valid number.");
            return false;
        }

        StructurizrClient structurizrClient = new StructurizrClient(apiKey.strip(), secret.strip());
        try {
            structurizrClient.putWorkspace(workspaceId, workspace);
        } catch (StructurizrClientException e) {
            log.error("Failed to upload workspace to Structurizr. Please check your credentials.", e);
            return false;
        }
        log.info("Workspace uploaded to Structurizr.");

        return true;
    }


    private WorkspaceUploader() {}

}
