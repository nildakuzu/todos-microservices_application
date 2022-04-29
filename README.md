# Todos Application
The aplication is developed to manage todos. Application structure is shown below.
ToDos![Todo-Application-Schema](https://user-images.githubusercontent.com/8994712/165810202-e0cedde3-3443-47b7-9105-3534f5257372.png)
<br />
<br />
## <b>Getting Started</b>
<br />
Make sure you have installed and configured docker in your environment. After that, you can run the below commands from the docker-compose.yml file directory and get started with the todos-microservices_application immediately.

- docker-compose build
- docker-compose up

# Services with ports
You can reach service endpoints like below
<br/>
![Screenshot 2022-04-28 203924](https://user-images.githubusercontent.com/8994712/165814276-cfbe0f65-c919-4da6-9eb3-f1cbe4370c71.png)

After you run commands, you can access it from the below specified addresses.

## Swagger endpoints
																	
- Web.Api.Gateway						      http://localhost:5000/swagger/index.html
- UserManagementService.Api			  http://localhost:5001/swagger/index.html				
- ToDoManagementService.Api			  http://localhost:5002/swagger/index.html				
- GroupManagementService.Api		  http://localhost:5003/swagger/index.html			
- ApiCompositionService				    http://localhost:5004/swagger/index.html
- NotificationService					    http://localhost:5005/swagger/index.html			

## External Application
- Redis								   http://localhost:8001
- Consul								 http://localhost:8500
- RabbitMQ							 http://localhost:15672/#/queues
