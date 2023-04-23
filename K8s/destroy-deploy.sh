#!/bin/bash

# Destroy CarService
cd CarService
./destroy-car.sh
cd ..

# Destroy Garage
cd GarageService
./destroy-garage.sh
cd ..

# Destroy User
cd UserService
./destroy-user.sh
cd ..

# Destroy Environment
cd EnvironmentService
./destroy-environment.sh
cd ..

#Ingress loadbalancer
kubectl delete deployment ingress-nginx-controller -n ingress-nginx

#RabbitMq
kubectl delete service rabbitmq-clusterip-srv
kubectl delete service rabbitmq-loadbalancer 
kubectl delete deployment rabbitmq-depl