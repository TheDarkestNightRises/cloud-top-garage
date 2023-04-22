#!/bin/bash

#RabbitMq
kubectl apply -f rabbitmq-depl.yaml 

#!/bin/bash

# Deploy CarService
cd CarService
./deploy-car.sh
cd ..

# Deploy Garage
cd Garage
./deploy-garage.sh
cd ..

# Deploy User
cd User
./deploy-user.sh
cd ..

# Deploy Environment
cd Environment
./deploy-environment.sh
cd ..


#Ingress nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.7.0/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingress-srv.yaml

