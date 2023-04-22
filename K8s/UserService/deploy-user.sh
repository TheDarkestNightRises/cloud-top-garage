#!/bin/bash

#User service
kubectl apply -f user-local-pvc.yaml 
kubectl apply -f user-mssql-initdb.yaml
kubectl apply -f users-mssql-depl.yaml
kubectl apply -f users-depl.yaml