#!/bin/bash

#------------Car Service--------------

# Deployments
kubectl delete deployment cars-depl
kubectl delete deployment cars-mssql-depl

# Services
kubectl delete service car-mssql-clusterip-srv
kubectl delete service cars-clusterip-srv
kubectl delete service cars-mssql-clusterip-srv
kubectl delete service cars-mssql-loadbalancer

#Ingress loadbalancer
kubectl delete deployment ingress-nginx-controller -n ingress-nginx