import json
import boto3

dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table('PongUsers')
    
def lambda_handler(event, context):
    
    try:
        createdUser = json.loads(event['body'])
        response = table.get_item( Key = {'user_id' : createdUser['user_id']} )
        
    except:
        return { 'statusCode': 200, 'body': json.dumps({"code":"0", "msg":"Failed to login."}) }
    
    try:
        msg = json.dumps(response['Item'])
        user = json.loads(msg)
    
        if createdUser['password'] != user['password']:
            return { 'statusCode': 200, 'body': json.dumps({"code":"1", "msg":"Wrong password."}) }
        else:
            return { 'statusCode': 200, 'body': msg }
            
    except: 
        
        try:
            if createdUser['user_id'] != "":
                table.put_item( Item={'user_id' : createdUser['user_id'], 'password': createdUser['password']} )
                return { 'statusCode': 200, 'body': json.dumps({"code":"2", "msg":"User created."}) }
        except:
             return { 'statusCode': 200, 'body': json.dumps({"code":"3", "msg":"Failed to create user."}) }