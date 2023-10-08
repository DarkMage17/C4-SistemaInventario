using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateC4();
        }

        static void GenerateC4()
        {
            const long workspaceId = 83243;
            const string apiKey = "db6bb54d-c5aa-481d-adfd-c5a499c2ee83";
            const string apiSecret = "abfbcf16-130b-412b-884d-989c9e226337";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Software Design - C4 Model - Sistema de Inventario", "Software Architecture Design Inventory System");
            Model model = workspace.Model;

            SoftwareSystem InventorySystem = model.AddSoftwareSystem("Sistema de Inventario", "Permite a los trabajores de la microempresa gestionar el inventario de productos");
            SoftwareSystem PredictiveModel = model.AddSoftwareSystem("Modelo Deep Learning", "Modelo de Deep Learning para la predicción de compra de materia prima");

            Person worker = model.AddPerson("Trabajador", "Trabajador de la microempresa");
            Person manager = model.AddPerson("Gerente", "Gerente de la microempresa");

            worker.Uses(InventorySystem, "Usa");
            manager.Uses(InventorySystem, "Usa");
            InventorySystem.Uses(PredictiveModel, "Usa para generar la predicción de compra de materia prima");
            
            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SystemContextView contextView = viewSet.CreateSystemContextView(InventorySystem, "Contexto", "Diagrama de Contexto");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            InventorySystem.AddTags("platformSafe");
            PredictiveModel.AddTags("stripeA");
            worker.AddTags("user");
            manager.AddTags("technical");

            //Styles
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("technical") { Background = "#03A9F4", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("user") { Background = "#03A9F4", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("platformSafe") { Background = "#009688", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("stripeA") { Background = "#6b0023", Color = "#ffffff", Shape = Shape.RoundedBox });            
            styles.Add(new ElementStyle("emailSystem") { Background = "#f568b5", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            
            Container singlePageApplication = InventorySystem.AddContainer("Single Page Application", "Provee toda la funcionalidad del sistema de inventario", "Angular");
            Container netApi = InventorySystem.AddContainer("API REST", "Permite a los usuarios de la aplicación gestionar el inventario de la microempresa", ".NET");
            
            Container predictionBoundedContext = InventorySystem.AddContainer("Prediction Bounded Context", "Bounded Context que permite el manejo de las predicciones de inventario", "");
            Container materialBoundedContext = InventorySystem.AddContainer("Material Bounded Context", "Bounded Context que gestiona los materiales del inventario", "");
            Container managerBoundedContext = InventorySystem.AddContainer("Manager Bounded Context", "Bounded Context que permite registrar los encargados", "");
            Container userBoundedContext = InventorySystem.AddContainer("Worker Bounded Context", "Bounded Context que permite registrar a los trabajadores", "");
            Container productBoundedContext = InventorySystem.AddContainer("Product Bounded Context", "Bounded Context que permite registrar los productos", "");

            Container dataBase = InventorySystem.AddContainer("Data Base", "Permite el almacenamiento de información", "SQL Server");

            singlePageApplication.Uses(netApi, "Usa");
        
            netApi.Uses(predictionBoundedContext, "Llamada API a");
            netApi.Uses(materialBoundedContext, "Llamada API a");
            netApi.Uses(managerBoundedContext, "Llamada API a");
            netApi.Uses(userBoundedContext, "Llamada API a");
            netApi.Uses(productBoundedContext, "Llamada API a");

            predictionBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            materialBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            managerBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            userBoundedContext.Uses(dataBase, "Lee desde y Escribe a");
            productBoundedContext.Uses(dataBase, "Lee desde y Escribe a");

            predictionBoundedContext.Uses(PredictiveModel, "Usa el modelo predictivo");
            manager.Uses(singlePageApplication, "Usa el sistema de inventario");
            worker.Uses(singlePageApplication, "Usa el sistema de inventario");

            //Tags
            singlePageApplication.AddTags("PageApp");
            netApi.AddTags("SpringAPI");
            
            predictionBoundedContext.AddTags("Appointment");
            materialBoundedContext.AddTags("Publication");
            managerBoundedContext.AddTags("TechnicalBC");
            userBoundedContext.AddTags("UserBC");
            productBoundedContext.AddTags("ProductBC");
            dataBase.AddTags("DataBase");
            
            //Styles
            styles.Add(new ElementStyle("WebApp") { Background = "#15ab92", Color = "#ffffff", Shape = Shape.WebBrowser});
            styles.Add(new ElementStyle("PageApp") { Background = "#9acd32", Color = "#ffffff", Shape = Shape.RoundedBox});
            styles.Add(new ElementStyle("SpringAPI") { Background = "#00276c", Color = "#ffffff", Shape = Shape.RoundedBox});
            
            styles.Add(new ElementStyle("Appointment") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("Publication") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("TechnicalBC") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });            
            styles.Add(new ElementStyle("UserBC") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });
            styles.Add(new ElementStyle("ProductBC") { Background = "#ff9800", Color = "#ffffff", Shape = Shape.Hexagon });
            styles.Add(new ElementStyle("DataBase") { Background = "#E00000", Color = "#ffffff", Shape = Shape.Cylinder });

            ContainerView containerView = viewSet.CreateContainerView(InventorySystem, "Contenedor", "Diagrama de contenedores");
            containerView.AddAllElements();;

            // ==================== Diagrama de componentes Prediction BC ====================
            Component predictionController = predictionBoundedContext.AddComponent("Prediction Controller", "Controlador que provee las respuestas de la Rest API para la prediccion", "");
            Component predictionService = predictionBoundedContext.AddComponent("Prediction Service", "Provee los métodos para la prediccion", "");
            Component predictionRepository = predictionBoundedContext.AddComponent("Prediction Repository", "Repositorio que provee los métodos para la persistencia de los datos de las predicciones.", "");
            Component predictionDomain = predictionBoundedContext.AddComponent("Prediction Domain Model", "Contiene todas las entidades del Bounded Context", "");

            netApi.Uses(predictionController, "Llamada API");
            predictionController.Uses(predictionService, "Llamada a los métodos del service");
            predictionService.Uses(predictionRepository, "Llamada a los métodos de persistencia del repository");
            predictionDomain.Uses(predictionRepository, "Conforma");
            predictionRepository.Uses(dataBase, "Lee desde y Escribe a");

            predictionService.Uses(PredictiveModel, "Usa el modelo predictivo");

            //Tags
            predictionController.AddTags("appointmentController");
            predictionService.AddTags("appointmentService");
            predictionRepository.AddTags("appointmentRepository");
            predictionDomain.AddTags("appointmentDomain");
            
            styles.Add(new ElementStyle("appointmentController") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentService") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentRepository") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("appointmentDomain") {Background = "#F58900", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView = viewSet.CreateComponentView(predictionBoundedContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A5_Landscape;
            componentView.Add(predictionBoundedContext);
            componentView.Add(netApi);
            componentView.Add(PredictiveModel);
            componentView.Add(singlePageApplication);
            componentView.Add(dataBase);
            componentView.AddAllComponents();

            // ==================== Diagrama de componentes Material BC ====================

            Component materialController = materialBoundedContext.AddComponent("Material Controller", "Controlador que provee las respuestas de la Rest API para la gestion de inventario", "");
            Component materialService = materialBoundedContext.AddComponent("Material Service", "Provee los métodos para la gestión de inventario", "");
            Component materialRepository = materialBoundedContext.AddComponent("Material Repository", "Repositorio que provee los métodos para la persistencia de los datos de los materiales.", "");
            Component materialDomain = materialBoundedContext.AddComponent("Material Domain Model", "Contiene todas las entidades del Bounded Context", "");
            
            netApi.Uses(materialController, "Llamada API");
            materialController.Uses(materialService, "Llamada a los métodos del service");
            materialService.Uses(materialRepository, "Llamada a los métodos de persistencia del repository");
            materialDomain.Uses(materialRepository, "Conforma");
            materialRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            materialController.AddTags("publicationController");
            materialService.AddTags("publicationService");
            materialRepository.AddTags("publicationRepository");
            materialDomain.AddTags("publicationDomain");

            styles.Add(new ElementStyle("publicationController") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationService") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationRepository") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("publicationDomain") {Background = "#760000", Color = "#ffffff", Shape = Shape.Component});

            ComponentView componentView2 = viewSet.CreateComponentView(materialBoundedContext, "Components2", "Component Diagram");
            componentView2.PaperSize = PaperSize.A5_Landscape;
            componentView2.Add(materialBoundedContext);
            componentView2.Add(netApi);
            componentView2.Add(singlePageApplication);
            componentView2.Add(dataBase);
            componentView2.AddAllComponents();

            // ==================== Diagrama de componentes Material BC ====================

            Component productController = productBoundedContext.AddComponent("Product Controller", "Controlador que provee las respuestas de la Rest API para la gestion de inventario", "");
            Component productService = productBoundedContext.AddComponent("Product Service", "Provee los métodos para la gestión de inventario", "");
            Component productRepository = productBoundedContext.AddComponent("Product Repository", "Repositorio que provee los métodos para la persistencia de los datos de los productos.", "");
            Component productDomain = productBoundedContext.AddComponent("Product Domain Model", "Contiene todas las entidades del Bounded Context", "");

            netApi.Uses(productController, "Llamada API");
            productController.Uses(productService, "Llamada a los métodos del service");
            productService.Uses(productRepository, "Llamada a los métodos de persistencia del repository");
            productDomain.Uses(productRepository, "Conforma");
            productRepository.Uses(dataBase, "Lee desde y Escribe a");

            //Tags
            productController.AddTags("productController");
            productService.AddTags("productService");
            productRepository.AddTags("productRepository");
            productDomain.AddTags("productDomain");

            styles.Add(new ElementStyle("productController") { Background = "#760000", Color = "#ffffff", Shape = Shape.Component });
            styles.Add(new ElementStyle("productService") { Background = "#760000", Color = "#ffffff", Shape = Shape.Component });
            styles.Add(new ElementStyle("productRepository") { Background = "#760000", Color = "#ffffff", Shape = Shape.Component });
            styles.Add(new ElementStyle("productDomain") { Background = "#760000", Color = "#ffffff", Shape = Shape.Component });

            ComponentView componentViewProduct = viewSet.CreateComponentView(productBoundedContext, "ProductComponent", "Component Diagram");
            componentViewProduct.PaperSize = PaperSize.A5_Landscape;
            componentViewProduct.Add(productBoundedContext);
            componentViewProduct.Add(netApi);
            componentViewProduct.Add(singlePageApplication);
            componentViewProduct.Add(dataBase);
            componentViewProduct.AddAllComponents();

            //Diagrama de componentes Technical BC
            Component technicalController = managerBoundedContext.AddComponent("Manager Controller", "Controlador que provee las respuestas de la Rest API para la gestion de encargados");
            Component technicalService = managerBoundedContext.AddComponent("Manager Service", "Provee los métodos para la inscripción y gestión de encargados");
            Component technicalRepository = managerBoundedContext.AddComponent("Manager Repository", "Provee los métodos para la persistencia de los datos de los encargados");
            Component technicalDomain = managerBoundedContext.AddComponent("Manager Domain Model", "Contiene todas las entidades del Bounded Context");
            Component technicalValidation = managerBoundedContext.AddComponent("Manager Validation", "Se encarga de validar que los datos del encargado son los correctos");
            
            netApi.Uses(technicalController, "Llamada API");
            technicalController.Uses(technicalService, "Llamada a los métodos del service");
            technicalService.Uses(technicalRepository, "Llamada a los métodos de persistencia del repository");
            technicalService.Uses(technicalValidation, "Llamada a los métodos de validación");
            technicalDomain.Uses(technicalRepository, "Conforma");
            technicalRepository.Uses(dataBase, "Lee desde y Escribe a");

            //Tags
            technicalController.AddTags("technicalController");
            technicalService.AddTags("technicalService");
            technicalRepository.AddTags("technicalRepository");
            technicalDomain.AddTags("technicalDomain");
            technicalValidation.AddTags("technicalValidation");
            
            styles.Add(new ElementStyle("technicalController") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalService") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalRepository") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalDomain") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("technicalValidation") {Background = "#6D4C41", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView3 = viewSet.CreateComponentView(managerBoundedContext, "Components3", "Component Diagram");
            componentView3.PaperSize = PaperSize.A5_Landscape;
            componentView3.Add(managerBoundedContext);
            componentView3.Add(singlePageApplication);
            componentView3.Add(netApi);
            componentView3.Add(dataBase);
            componentView3.AddAllComponents();
            
            //Diagrama de componentes User BC
            Component userController = userBoundedContext.AddComponent("Worker Controller", "Controlador que provee las respuestas de la Rest API para la gestion de trabajadores");
            Component userService = userBoundedContext.AddComponent("Worker Service", "Provee los métodos para la inscripción y gestión de trabajadores");
            Component userRepository = userBoundedContext.AddComponent("Worker Repository", "Provee los métodos para la persistencia de los datos de los trabajadores");
            Component userDomain = userBoundedContext.AddComponent("Worker Domain Model", "Contiene todas las entidades del Bounded Context");
            Component userValidation = userBoundedContext.AddComponent("Worker Validation", "Se encarga de validar que los datos del trabajador son los correctos");

            netApi.Uses(userController, "Llamada API");
            userController.Uses(userService, "Llamada a los métodos del service");
            userService.Uses(userRepository, "Llamada a los métodos de persistencia del repository");
            userService.Uses(userValidation, "Llamada a los métodos de validación");
            userDomain.Uses(userRepository, "Conforma");
            userRepository.Uses(dataBase, "Lee desde y Escribe a");
            
            //Tags
            userController.AddTags("userController");
            userService.AddTags("userService");
            userRepository.AddTags("userRepository");
            userDomain.AddTags("userDomain");
            userValidation.AddTags("userValidation");
            
            styles.Add(new ElementStyle("userController") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userService") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userRepository") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userDomain") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            styles.Add(new ElementStyle("userValidation") {Background = "#50126D", Color = "#ffffff", Shape = Shape.Component});
            
            ComponentView componentView4 = viewSet.CreateComponentView(userBoundedContext, "Components4", "Component Diagram");
            componentView4.PaperSize = PaperSize.A4_Landscape;

            contextView.PaperSize = PaperSize.A5_Portrait;
            containerView.PaperSize = PaperSize.A4_Landscape;

            componentView4.Add(userBoundedContext);
            componentView4.Add(singlePageApplication);
            componentView4.Add(netApi);
            componentView4.Add(dataBase);
            componentView4.AddAllComponents();
            
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}