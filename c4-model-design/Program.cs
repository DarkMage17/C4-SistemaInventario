using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 76872;
            const string apiKey = "API_KEY";
            const string apiSecret = "API_SECRET";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Software Design - C4 Model - Safe Technology", "Software Architecture Design Safe Technology");
            Model model = workspace.Model;
            
            SoftwareSystem safePlatform = model.AddSoftwareSystem("Safe Technology Platform", "Permite a los usuarios visualizar información y contactar con técnicos disponibles");
            SoftwareSystem stripeApi = model.AddSoftwareSystem("Stripe API", "API de la empresa Paypal que permite pagos de manera online");
            SoftwareSystem emailSystem = model.AddSoftwareSystem("E-mail System", "Sistema de e-mail que envía un correo de confirmación de cuenta");

            Person user = model.AddPerson("User", "Usuario de la plataforma");
            Person technical = model.AddPerson("Technical", "Usuario que presta sus servicios mediante la plataforma");

            user.Uses(safePlatform, "Usa");
            technical.Uses(safePlatform, "Usa");
            safePlatform.Uses(stripeApi, "Permite los pagos dentro de la plataforma");
            safePlatform.Uses(emailSystem, "Envía e-mail de verificación de cuenta");
            emailSystem.Delivers(user, "Envía correo a");
            emailSystem.Delivers(technical, "Envía correo a");
            stripeApi.Delivers(user, "Envía constancia de pago a");
            stripeApi.Delivers(technical, "Envía constancia de pagos a");
            
            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SystemContextView contextView = viewSet.CreateSystemContextView(safePlatform, "Contexto", "Diagrama de Contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            safePlatform.AddTags("platformSafe");
            stripeApi.AddTags("stripeA");
            emailSystem.AddTags("emailSystem");
            user.AddTags("user");
            technical.AddTags("technical");

            //Styles
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("technical") { Background = "#03A9F4", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("user") { Background = "#03A9F4", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("platformSafe") { Background = "#009688", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("stripeA") { Background = "#6b0023", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("emailSystem") { Background = "#f568b5", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            
            Container webApplication = safePlatform.AddContainer("Web Application", "Entrega contenido estático y la página simple de Safe Technology Platform", "Spring Java");
            Container singlePageApplication = safePlatform.AddContainer("Single Page Application", "Provee toda la funcionalidad de la plataforma de Safe Technology a los técnicos", "Angular");
            Container springBootApi = safePlatform.AddContainer("API REST", "Permite a los usuarios de la aplicación conectarse e interactuar, compartiendo y agregando información", "SpringBoot");
            
            Container appointmentBoundedContext = safePlatform.AddContainer("Appointment Bounded Context", "Bounded Context que permite el registro de citas técnicas entre usuarios y técnicos", "");
            Container publicationBoundedContext = safePlatform.AddContainer("Publication Bounded Context", "Bounded Context que gestiona las publicaciones de los técnicos", "");
            Container technicalBoundedContext = safePlatform.AddContainer("Technical Bounded Context", "Bounded Context que permite registrar a los técnicos", "");
            Container userBoundedContext = safePlatform.AddContainer("User Bounded Context", "Bounded Context que permite registrar a los usuarios", "");
 
            
            Container dataBase = safePlatform.AddContainer("Data Base", "Permite el almacenamiento de información", "MySQL");

            user.Uses(webApplication, "Buscar recomendaciones y consejos sobre reparaciones de electrodomésticos");
            technical.Uses(webApplication, "Prestar sus servicios como técnico usando la plataforma");

            webApplication.Uses(singlePageApplication, "Entrega al navegador web del cliente");
            singlePageApplication.Uses(springBootApi, "Usa");
            springBootApi.Uses(emailSystem, "Envía correo de verificación");
        

            springBootApi.Uses(appointmentBoundedContext, "Llamada API a");
            springBootApi.Uses(publicationBoundedContext, "Llamada API a");
            springBootApi.Uses(technicalBoundedContext, "Llamada API a");
            springBootApi.Uses(userBoundedContext, "Llamada API a");

            appointmentBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            publicationBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            technicalBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            userBoundedContext.Uses(dataBase, "Lee desde y Escribe a");


            //Tags
            webApplication.AddTags("WebApp");
            singlePageApplication.AddTags("PageApp");
            springBootApi.AddTags("SpringAPI");
            
            appointmentBoundedContext.AddTags("Appointment");
            publicationBoundedContext.AddTags("Publication");
            technicalBoundedContext.AddTags("TechnicalBC");
            userBoundedContext.AddTags("UserBC");
            dataBase.AddTags("DataBase");
            
            //Styles
            styles.Add(new ElementStyle("WebApp") { Background = "#15ab92", Color = "#ffffff", Shape = Shape.WebBrowser});
            styles.Add(new ElementStyle("PageApp") { Background = "#9acd32", Color = "#ffffff", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle("SpringAPI") { Background = "#00276c", Color = "#ffffff", Shape = Shape.RoundedBox});
            
            styles.Add(new ElementStyle("Appointment") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Publication") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("TechnicalBC") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("UserBC") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("DataBase") { Background = "#E00000", Color = "#ffffff", Shape = Shape.Cylinder });

            ContainerView containerView = viewSet.CreateContainerView(safePlatform, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A3_Landscape;
            containerView.AddAllElements();  
            
            //Diagrama de componentes Appointment BC
            Component appointmentController = appointmentBoundedContext.AddComponent("Appointment Controller", "Controlador que provee los Rest API para la gestión de citas", "");
            Component appointmentService = appointmentBoundedContext.AddComponent("Appointment Service", "Provee los métodos para la inscripción y gestión de citas", "");
            Component appointmentRepository = appointmentBoundedContext.AddComponent("Appointment Repository", "Repositorio que provee los métodos para la persistencia de los datos de las citas.", "");
            Component appointmentDomain = appointmentBoundedContext.AddComponent("Appointment Domain Model", "Contiene todas las entidades del Bounded Context", "");

            springBootApi.Uses(appointmentController, "Llamada API");
            appointmentController.Uses(appointmentService, "Llamada a los métodos del service");
            appointmentService.Uses(appointmentRepository, "Llamada a los métodos de persistencia del repository");
            appointmentDomain.Uses(appointmentRepository, "Conforma");
            appointmentRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            appointmentController.AddTags("appointmentController");
            appointmentService.AddTags("appointmentService");
            appointmentRepository.AddTags("appointmentRepository");
            appointmentDomain.AddTags("appointmentDomain");
            
            styles.Add(new ElementStyle("appointmentController") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentService") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentRepository") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentDomain") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView = viewSet.CreateComponentView(appointmentBoundedContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(appointmentBoundedContext);
            componentView.Add(springBootApi);
            componentView.Add(singlePageApplication);
            componentView.Add(dataBase);
            componentView.AddAllComponents();
           
            //Diagrama de componentes Publication BC
            
            Component publicationController = publicationBoundedContext.AddComponent("Publication Controller", "Controlador que provee los Rest API para la gestión de publiaciones", "");
            Component publicationService = publicationBoundedContext.AddComponent("Publication Service", "Provee los métodos para la gestión de publicaciones", "");
            Component publicationRepository = publicationBoundedContext.AddComponent("Publication Repository", "Repositorio que provee los métodos para la persistencia de los datos de las publicaciones.", "");
            Component publicationDomain = publicationBoundedContext.AddComponent("Publication Domain Model", "Contiene todas las entidades del Bounded Context", "");
            
            Component tagController = publicationBoundedContext.AddComponent("Tag Controller", "Controlador que provee los Rest API para la gestión de tags", "");
            Component tagService = publicationBoundedContext.AddComponent("Tag Service", "Provee los métodos para la gestión de tags", "");
            Component tagRepository = publicationBoundedContext.AddComponent("Tag Repository", "Repositorio que provee los métodos para la persistencia de los datos de los tags.", "");
            Component tagDomain = publicationBoundedContext.AddComponent("Tag Domain Model", "Contiene todas las entidades del Bounded Context", "");

            Component commentController = publicationBoundedContext.AddComponent("Comment Controller", "Controlador que provee los Rest API para la gestión de comments", "");
            Component commentService = publicationBoundedContext.AddComponent("Comment Service", "Provee los métodos para la gestión de comments", "");
            Component commentRepository = publicationBoundedContext.AddComponent("Comment Repository", "Repositorio que provee los métodos para la persistencia de los datos de los comments.", "");
            Component commentDomain = publicationBoundedContext.AddComponent("Comment Domain Model", "Contiene todas las entidades del Bounded Context", "");

            
            springBootApi.Uses(publicationController, "Llamada API");
            publicationController.Uses(publicationService, "Llamada a los métodos del service");
            publicationService.Uses(publicationRepository, "Llamada a los métodos de persistencia del repository");
            publicationDomain.Uses(publicationRepository, "Conforma");
            publicationRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            springBootApi.Uses(tagController, "Llamada API");
            tagController.Uses(tagService, "Llamada a los métodos del service");
            tagService.Uses(tagRepository, "Llamada a los métodos de persistencia del repository");
            tagDomain.Uses(tagRepository, "Conforma");
            tagRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            springBootApi.Uses(commentController, "Llamada API");
            commentController.Uses(commentService, "Llamada a los métodos del service");
            commentService.Uses(commentRepository, "Llamada a los métodos de persistencia del repository");
            commentDomain.Uses(commentRepository, "Conforma");
            commentRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            publicationController.AddTags("publicationController");
            publicationService.AddTags("publicationService");
            publicationRepository.AddTags("publicationRepository");
            publicationDomain.AddTags("publicationDomain");
            
            tagController.AddTags("tagController");
            tagService.AddTags("tagService");
            tagRepository.AddTags("tagRepository");
            tagDomain.AddTags("tagDomain");
            
            commentController.AddTags("commentController");
            commentService.AddTags("commentService");
            commentRepository.AddTags("commentRepository");
            commentDomain.AddTags("commentDomain");
            
            styles.Add(new ElementStyle("publicationController") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationService") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationRepository") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationDomain") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            
            styles.Add(new ElementStyle("tagController") {Background = "#606DDB", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("tagService") {Background = "#606DDB", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("tagRepository") {Background = "#606DDB", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("tagDomain") {Background = "#606DDB", Color = "#ffffff", Shape = Shape.Component});
            
            styles.Add(new ElementStyle("commentController") {Background = "#9D0C4A", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("commentService") {Background = "#9D0C4A", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("commentRepository") {Background = "#9D0C4A", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("commentDomain") {Background = "#9D0C4A", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView2 = viewSet.CreateComponentView(publicationBoundedContext, "Components2", "Component Diagram");
            componentView2.PaperSize = PaperSize.A3_Landscape;
            componentView2.Add(publicationBoundedContext);
            componentView2.Add(springBootApi);
            componentView2.Add(singlePageApplication);
            componentView2.Add(dataBase);
            componentView2.AddAllComponents();
            
            //Diagrama de componentes Technical BC
            Component technicalController = technicalBoundedContext.AddComponent("Technical Controller", "Provee las RestAPI para el manejo de técnicos");
            Component technicalService = technicalBoundedContext.AddComponent("Technical Service", "Provee los métodos para la inscripción y gestión de técnicos");
            Component technicalRepository = technicalBoundedContext.AddComponent("Technical Repository", "Provee los métodos para la persistencia de los datos de los técnicos");
            Component technicalDomain = technicalBoundedContext.AddComponent("Technical Domain Model", "Contiene todas las entidades del Bounded Context");
            Component technicalValidation = technicalBoundedContext.AddComponent("Technical Validation", "Se encarga de validar que los datos del técnico son los correctos");
            
            Component applianceController = technicalBoundedContext.AddComponent("Appliance Controller", "Provee las RestAPI para el manejo de aparatos");
            Component applianceService = technicalBoundedContext.AddComponent("Appliance Service", "Provee los métodos para la inscripción y gestión de aparatos");
            Component applianceRepository = technicalBoundedContext.AddComponent("Appliance Repository", "Provee los métodos para la persistencia de los datos de aparatos");
            Component applianceDomain = technicalBoundedContext.AddComponent("Appliance Domain Model", "Contiene todas las entidades del Bounded Context");

            Component scheduleController = technicalBoundedContext.AddComponent("Schedule Controller", "Provee las RestAPI para el manejo de horarios");
            Component scheduleService = technicalBoundedContext.AddComponent("Schedule Service", "Provee los métodos para la inscripción y gestión de horarios");
            Component scheduleRepository = technicalBoundedContext.AddComponent("Schedule Repository", "Provee los métodos para la persistencia de los datos de los horarios");
            Component scheduleDomain = technicalBoundedContext.AddComponent("Schedule Domain Model", "Contiene todas las entidades del Bounded Context");
            
            springBootApi.Uses(technicalController, "Llamada API");
            technicalController.Uses(technicalService, "Llamada a los métodos del service");
            technicalService.Uses(technicalRepository, "Llamada a los métodos de persistencia del repository");
            technicalService.Uses(technicalValidation, "Llamada a los métodos de validación");
            technicalDomain.Uses(technicalRepository, "Conforma");
            technicalRepository.Uses(dataBase, "Lee desde y Escribe a");

            springBootApi.Uses(applianceController, "Llamada API");
            applianceController.Uses(applianceService, "Llamada a los métodos del service");
            applianceService.Uses(applianceRepository, "Llamada a los métodos de persistencia del repository");
            applianceDomain.Uses(applianceRepository, "Conforma");
            applianceRepository.Uses(dataBase, "Lee desde y Escribe a");

            springBootApi.Uses(scheduleController, "Llamada API");
            scheduleController.Uses(scheduleService, "Llamada a los métodos del service");
            scheduleService.Uses(scheduleRepository, "Llamada a los métodos de persistencia del repository");
            scheduleDomain.Uses(scheduleRepository, "Conforma");
            scheduleRepository.Uses(dataBase, "Lee desde y Escribe a");

            //Tags
            technicalController.AddTags("technicalController");
            technicalService.AddTags("technicalService");
            technicalRepository.AddTags("technicalRepository");
            technicalDomain.AddTags("technicalDomain");
            technicalValidation.AddTags("technicalValidation");
            
            applianceController.AddTags("applianceController");
            applianceService.AddTags("applianceService");
            applianceRepository.AddTags("applianceRepository");
            applianceDomain.AddTags("applianceDomain");
            
            scheduleController.AddTags("scheduleController");
            scheduleService.AddTags("scheduleService");
            scheduleRepository.AddTags("scheduleRepository");
            scheduleDomain.AddTags("scheduleDomain");
            
            styles.Add(new ElementStyle("technicalController") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalService") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalRepository") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalDomain") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalValidation") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            
            styles.Add(new ElementStyle("applianceController") {Background = "#96876F", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("applianceService") {Background = "#96876F", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("applianceRepository") {Background = "#96876F", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("applianceDomain") {Background = "#96876F", Color = "#ffffff", Shape = Shape.Component});
            
            styles.Add(new ElementStyle("scheduleController") {Background = "#889E0D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("scheduleService") {Background = "#889E0D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("scheduleRepository") {Background = "#889E0D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("scheduleDomain") {Background = "#889E0D", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView3 = viewSet.CreateComponentView(technicalBoundedContext, "Components3", "Component Diagram");
            componentView3.PaperSize = PaperSize.A3_Landscape;
            componentView3.Add(technicalBoundedContext);
            componentView3.Add(singlePageApplication);
            componentView3.Add(springBootApi);
            componentView3.Add(dataBase);
            componentView3.AddAllComponents();
            
            //Diagrama de componentes User BC
            Component userController = userBoundedContext.AddComponent("User Controller", "Provee las RestAPI para el manejo de usuarios");
            Component userService = userBoundedContext.AddComponent("User Service", "Provee los métodos para la inscripción y gestión de usuarios");
            Component userRepository = userBoundedContext.AddComponent("User Repository", "Provee los métodos para la persistencia de los datos de los usuarios");
            Component userDomain = userBoundedContext.AddComponent("User Domain Model", "Contiene todas las entidades del Bounded Context");
            Component userValidation = userBoundedContext.AddComponent("User Validation", "Se encarga de validar que los datos del usuario son los correctos");
            
            Component reviewController = userBoundedContext.AddComponent("Review Controller", "Provee las RestAPI para el manejo de reseñas");
            Component reviewService = userBoundedContext.AddComponent("Review Service", "Provee los métodos para la inscripción y gestión de reseñas");
            Component reviewRepository = userBoundedContext.AddComponent("Review Repository", "Provee los métodos para la persistencia de los datos de los reseñas");
            Component reviewDomain = userBoundedContext.AddComponent("Review Domain Model", "Contiene todas las entidades del Bounded Context");

            springBootApi.Uses(userController, "Llamada API");
            userController.Uses(userService, "Llamada a los métodos del service");
            userService.Uses(userRepository, "Llamada a los métodos de persistencia del repository");
            userService.Uses(userValidation, "Llamada a los métodos de validación");
            userDomain.Uses(userRepository, "Conforma");
            userRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            springBootApi.Uses(reviewController, "Llamada API");
            reviewController.Uses(reviewService, "Llamada a los métodos del service");
            reviewService.Uses(reviewRepository, "Llamada a los métodos de persistencia del repository");
            reviewDomain.Uses(reviewRepository, "Conforma");
            reviewRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            userController.AddTags("userController");
            userService.AddTags("userService");
            userRepository.AddTags("userRepository");
            userDomain.AddTags("userDomain");
            userValidation.AddTags("userValidation");
            
            reviewController.AddTags("reviewController");
            reviewService.AddTags("reviewService");
            reviewRepository.AddTags("reviewRepository");
            reviewDomain.AddTags("reviewDomain");
            
            styles.Add(new ElementStyle("userController") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userService") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userRepository") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userDomain") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userValidation") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            
            styles.Add(new ElementStyle("reviewController") {Background = "#0571ED", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("reviewService") {Background = "#0571ED", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("reviewRepository") {Background = "#0571ED", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("reviewDomain") {Background = "#0571ED", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView4 = viewSet.CreateComponentView(userBoundedContext, "Components4", "Component Diagram");
            componentView4.PaperSize = PaperSize.A4_Landscape;
            componentView4.Add(userBoundedContext);
            componentView4.Add(singlePageApplication);
            componentView4.Add(springBootApi);
            componentView4.Add(dataBase);
            componentView4.AddAllComponents();
            
            
            
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}