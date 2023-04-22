#!/bin/bash

#------------Car Service--------------
# Deployments
kubectl delete deployment cars-depl
kubectl delete deployment cars-mssql-depl

# Services
kubectl delete service cars-clusterip-srv
kubectl delete service cars-mssql-clusterip-srv
kubectl delete service cars-mssql-loadbalancer
