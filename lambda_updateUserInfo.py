import json
import boto3

dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table('PongUsers')
    
def lambda_handler(event, context):
    
    try:
        updateUser = json.loads( event['body'] )
        response = table.get_item( Key = {'user_id' : updateUser['user_id']} )
        item = response['Item']
        item['score'] = updateUser['score']
        table.put_item( Item=item )
        return { 'statusCode': 200, 'body': json.dumps({"code":"0", "msg":"Good to update."}) }
        
        
        
    except:
        return { 'statusCode': 200, 'body': json.dumps({"code":"0", "msg":"Failed to update."}) }