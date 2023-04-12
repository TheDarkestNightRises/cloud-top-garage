#!/bin/bash

#Deployments
kubectl apply -f cars-depl.yaml

#Database
kubectl apply -f car-local-pvc.yaml
kubectl apply -f cars-mssql-depl.yaml

# Dont forget to kubectl create secret generic car-mssql --from-literal=SA_PASSWORD="pa55w0rd"

#Ingress nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.6.4/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingress-srv.yaml