#!/bin/bash

#------------Car Service--------------
# Deployments
kubectl delete deployment cars-depl
kubectl delete deployment cars-mssql-depl

# Services
kubectl delete service cars-clusterip-srv
kubectl delete service cars-mssql-clusterip-srv
kubectl delete service cars-mssql-loadbalancer

#------------User Service--------------
# Deployments
kubectl delete deployment users-depl
kubectl delete deployment users-mssql-depl

# Services
kubectl delete service users-clusterip-srv
kubectl delete service users-mssql-clusterip-srv
kubectl delete service users-mssql-loadbalancer

#------------Environment Service--------------
# Deployments
kubectl delete deployment environments-depl
kubectl delete deployment environments-mssql-depl

# Services
kubectl delete service environments-clusterip-srv
kubectl delete service environments-mssql-clusterip-srv
kubectl delete service environments-mssql-loadbalancer

#------------Garage Service--------------
# Deployments
kubectl delete deployment garages-depl
kubectl delete deployment garages-mssql-depl

# Services
kubectl delete service garages-clusterip-srv
kubectl delete service garages-mssql-clusterip-srv
kubectl delete service garages-mssql-loadbalancer

#Ingress loadbalancer
kubectl delete deployment ingress-nginx-controller -n ingress-nginx