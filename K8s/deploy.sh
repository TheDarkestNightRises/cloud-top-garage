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


#Ingress nginx
helm upgrade --install ingress-nginx ingress-nginx --repo https://kubernetes.github.io/ingress-nginx --namespace ingress-nginx --create-namespace
#If Applying the Custom Ingress doesnt work try: kubectl delete -A ValidatingWebhookConfiguration ingress-nginx-admission
kubectl apply -f ingress-srv.yaml

