#!/bin/bash

#RabbitMq
kubectl apply -f rabbitmq-depl.yaml 

# Deploy CarService
cd CarService
./deploy-car.sh
cd ..

# Deploy Garage
cd GarageService
./deploy-garage.sh
cd ..

# Deploy User
cd UserService
./deploy-user.sh
cd ..

# Deploy Environment
cd EnvironmentService
./deploy-environment.sh
cd ..

#deploy-traefik
cd Traefik
./deploy-traefik.sh
