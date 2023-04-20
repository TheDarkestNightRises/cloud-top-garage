#!/bin/bash

#Car Service
kubectl apply -f car-local-pvc.yaml
kubectl apply -f car-mssql-initdb.yaml
kubectl apply -f cars-mssql-depl.yaml
kubectl apply -f cars-depl.yaml

#User service
kubectl apply -f user-local-pvc.yaml 
kubectl apply -f user-mssql-initdb.yaml
kubectl apply -f users-mssql-depl.yaml
kubectl apply -f users-depl.yaml

#Environment service
kubectl apply -f environment-local-pvc.yaml 
kubectl apply -f environment-mssql-initdb.yaml
kubectl apply -f environments-mssql-depl.yaml
kubectl apply -f environments-depl.yaml

#Garage Service
kubectl apply -f garage-local-pvc.yaml
kubectl apply -f garage-mssql-initdb.yaml
kubectl apply -f garages-mssql-depl.yaml
kubectl apply -f garages-depl.yaml


# Dont forget to kubectl create secret generic car-mssql --from-literal=SA_PASSWORD="Pa55w0rd!"

#Ingress nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.7.0/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingress-srv.yaml

# # Ocelot gateway
# kubectl apply -f ocelot-config.yaml
# kubectl apply -f ocelot-depl.yaml
# kubectl apply -f ocelot-ingress.yaml