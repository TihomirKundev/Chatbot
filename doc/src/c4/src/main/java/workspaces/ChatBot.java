package workspaces;

import com.structurizr.Workspace;
import com.structurizr.model.*;
import com.structurizr.view.*;

public class ChatBot
{
    private static final String DATABASE_TAG = "Database";
    private static final String WEB_BROWSER_TAG = "Web Browser";

    public Workspace getWorkspace()
    {
        var workspace = new Workspace(
                this.getClass().getSimpleName(),
                "");

        Model model = workspace.getModel();
        ViewSet views = workspace.getViews();

        Person customer = model.addPerson(
                "Customer");

        Person customerServiceAgent = model.addPerson(
                "Customer Service Agent");


        //// System context view (C1)
        SoftwareSystem chatBot = model.addSoftwareSystem(
                "Chat Bot main system",
                "A chat-bot system that allows customers to receive customer support via a chat interface.");

        SoftwareSystem mockBasWorldApi = model.addSoftwareSystem(
                "Mock BasWorld Service",
                "A mock API that provides information about the customer's account.");

        SoftwareSystem aiService = model.addSoftwareSystem(
                "AI Service",
                "Generates responses to customer messages.");


        customer.uses(chatBot, "Requests information using");
        customerServiceAgent.uses(chatBot, "Manages customer service requests using");
        chatBot.uses(mockBasWorldApi, "Retrieves company and customer information from");
        chatBot.uses(aiService, "Uses to understand customer requests");

        SystemContextView contextView = views.createSystemContextView(
                chatBot,
                "System Context",
                "");
        contextView.addAllSoftwareSystems();
        contextView.addAllPeople();


        //// Container view (C2) - Chat Bot Main System
        Container singlePageApplication = chatBot.addContainer(
                "Single-Page application",
                "Chat interface for customers to interact with the chat bot.",
                "React");
        singlePageApplication.addTags(WEB_BROWSER_TAG);

        Container apiApplication = chatBot.addContainer(
                "Chat Bot Application",
                "REST API that handles user requests.",
                "ASP.NET 6");

        Container database = chatBot.addContainer(
                "Database",
                "Stores conversation history.",
                "PostgreSQL + Entity Framework");
        database.addTags(DATABASE_TAG);

        customer.uses(singlePageApplication, "Uses");
        customerServiceAgent.uses(singlePageApplication, "Uses");
        singlePageApplication.uses(apiApplication, "Sends requests to");
        apiApplication.uses(database, "Reads from and writes to");

        apiApplication.uses(mockBasWorldApi, "Retrieves company and customer information from");
        apiApplication.uses(aiService, "Uses to understand customer requests");
        ContainerView containerView = views.createContainerView(
                chatBot,
                "Chat Bot main system container view",
                "");

        containerView.add(customer);
        containerView.add(customerServiceAgent);
        containerView.add(singlePageApplication);
        containerView.add(apiApplication);
        containerView.add(database);
        containerView.add(mockBasWorldApi);
        containerView.add(aiService);


        // Styles
        Styles styles = views.getConfiguration().getStyles();
        styles.addElementStyle(Tags.PERSON).background("#08427b").color("#ffffff").shape(Shape.Person);
        styles.addElementStyle(Tags.SOFTWARE_SYSTEM).background("#1168bd").color("#ffffff");
        styles.addElementStyle(Tags.CONTAINER).background("#438dd5").color("#ffffff");
        styles.addElementStyle(Tags.COMPONENT).background("#85bbf0").color("#000000");
        styles.addElementStyle(DATABASE_TAG).shape(Shape.Cylinder);
        styles.addElementStyle(WEB_BROWSER_TAG).shape(Shape.WebBrowser);

        // Auto-layout
        contextView.enableAutomaticLayout();

        return workspace;
    }
}
