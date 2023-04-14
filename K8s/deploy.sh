#!/bin/bash

#Car Service
kubectl apply -f cars-depl.yaml
kubectl apply -f car-local-pvc.yaml
kubectl apply -f cars-mssql-depl.yaml

#User service
kubectl apply -f users-depl.yaml
kubectl apply -f user-local-pvc.yaml 
kubectl apply -f users-mssql-depl.yaml

# Dont forget to kubectl create secret generic car-mssql --from-literal=SA_PASSWORD="Pa55w0rd!"

#Ingress nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.6.4/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingress-srv.yaml