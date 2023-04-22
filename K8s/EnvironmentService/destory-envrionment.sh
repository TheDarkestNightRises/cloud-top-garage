#!/bin/bash

#------------Environment Service--------------
# Deployments
kubectl delete deployment environments-depl
kubectl delete deployment environments-mssql-depl

# Services
kubectl delete service environments-clusterip-srv
kubectl delete service environments-mssql-clusterip-srv
kubectl delete service environments-mssql-loadbalancer