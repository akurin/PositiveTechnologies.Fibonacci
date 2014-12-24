PositiveTechnologies.Fibonacci
==============================

How to start application:

1. Install RabbitMQ
2. Run "PositiveTechnologies.Fibonacci.CalculationService.exe http://localhost:9000 rabbitmq://localhost/fibonacci". There is no need to deploy anything to IIS. PositiveTechnologies.Fibonacci.CalculationService is a self-hosting Web Application.
3. Run "PositiveTechnologies.Fibonacci.ConsoleClient.exe 3 http://localhost:9000 rabbitmq://localhost/fibonacci"

After a few seconds PositiveTechnologies.Fibonacci.ConsoleClient will display error messages. It's OK. Application doesn't use any sort of arbitrary-precision arithmetic therefore overflow happens. 
