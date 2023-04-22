#!/bin/bash

#Car Service
kubectl apply -f car-mssql.yaml
kubectl apply -f car-local-pvc.yaml
kubectl apply -f car-mssql-initdb.yaml
kubectl apply -f cars-mssql-depl.yaml
kubectl apply -f cars-depl.yaml