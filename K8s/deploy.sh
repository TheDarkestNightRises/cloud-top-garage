#!/bin/bash

#Deployments
kubectl apply -f cars-depl.yaml
#Ingress nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.6.4/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingress-srv.yaml