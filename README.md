# APILast
A Interprocess Hook for messagebased Services that generates ASP.NET Controllers for Swagger

So you have Services and in the cool and fancy decoupled world they all communicate async with Messages.

How can you develop this kind of Service ? 
Do you generate Messages in NServiceBus, MSMQ or RabbitMQ and hope that you didnt mess something up and 
susbcried to the Prod Queue ? 
What about out of order Delivery ? And Error Queues ? 
Whatever i really dont like this way off Developing. Having workaround over workaround and messy configuration 
on Developer Machine level (oO dont check it in or you get messages from somebody else)
All the advantages of Message Queuing getting disadvantages while developing - or QA Testing. 

A different way is to have HTTP Endpoints for each Operation.
So you are forced to use ASP.Net MVC! 
(I am from the C# world so there only exists ASP.Net as WebApplication Framework for me)
So you write operation after operation and also some Controller Plumbing Code. 

Okay HTTP Endpoint - done, but its Productive Code which could cause issues and is also useless for 
Prod as it only exists for QA and Development. 
So you should take care of HTTPS because nobody except the QA should trigger the service Blackbox to do 
something. Which brings authentification and .... 

It just getting more complicated or there is just more boilerplate.


## What should be the flow 

1. develop a service as (windows)service because it does not need HTTP endpoints
2. hit refresh on your local deployed Instance of APILast. 
3. see the updated SWAGGER Contract of your Operation  
4. use SOAP UI (or product of your choice to genrate HTTP Requests) to create some message
5. get a Response 
6. run assertions

## What i want to achive 

### Init

1. APILast will hook into the service process
2. inject some required libs 
3. establish Interprocess Communication through named Pipes
4. Export types and namespaces of service 
5. load required Assembies on APILast
6. generate Controller Classes from Types and add to new assembly  
7. register the new Controllers 

### Use It

1. ApiExplorer will hopefully pickup the new Controllers 
2. request to controller will modelBind the concrete class from Service
3. this Model instance is pushed to the NamedPipe 
4. Service Process recieves the Message from NamedPipe
5. resolve the MessageHandler from Windsor 
6. invoke the MessageHandler
7. if exists push the MessageHandler response to the NamedPipe
8. controller is waiting for this 
9. render response
10. done


 






 

