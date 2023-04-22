#!/bin/bash

#------------Garage Service--------------
# Deployments
kubectl delete deployment garages-depl
kubectl delete deployment garages-mssql-depl

# Services
kubectl delete service garages-clusterip-srv
kubectl delete service garages-mssql-clusterip-srv
kubectl delete service garages-mssql-loadbalancer
