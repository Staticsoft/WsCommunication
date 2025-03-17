param(
    [string] $Stage = 'Dev'
)

$StackName = "WebSocketCommunication${Stage}BackendLambda"
$Local = "$PSScriptRoot/../.local"

function Publish-Code {
    & dotnet @(
        'publish'
        "$PSScriptRoot/../code/TestServer.AWS/TestServer.AWS.csproj"
        '-c'
        'Release'
        '-o'
        "$Local/Backend"
        '-r'
        'linux-x64'
    )
}

function Publish-Template {
    $bucketName = "staticsoft.artifacts"

    & aws @(
        'cloudformation'
        'package'
        '--template-file'
        "$PSScriptRoot/templates/Backend.yml"
        '--s3-bucket'
        $bucketName
        '--output-template-file'
        "$Local/Backend.yml"
    )
}

function Get-ExportValue {
    param(
        [string] $name
    )

    & aws @(
        'cloudformation'
        'list-exports'
        '--query'
        "(Exports[?Name=='$name'].Value)[0]"
        '--output'
        'text'
    )
}

function Deploy-Stack {
    $stack = Find-Stack
    if ($null -eq $stack) {
        New-Stack
    }
    else {
        Update-Stack
    }
}

function Find-Stack {
    & aws @(
        'cloudformation'
        'describe-stacks'
        '--stack-name'
        $StackName
    ) 2>$null
}

function New-Stack {
    Write-Stack -Command 'create-stack'
    Wait-Stack -Condition 'stack-create-complete'
}

function Update-Stack {
    Write-Stack -Command 'update-stack'
    Wait-Stack -Condition 'stack-update-complete'
}

function Write-Stack {
    param(
        [string] $command
    )

    & aws @(
        'cloudformation'
        $command
        '--stack-name'
        $StackName
        '--template-body'
        "file://$Local/Backend.yml"
        '--parameters'
        "ParameterKey=Stage,ParameterValue=$Stage"
        "--tags=Key=product,Value=WebSocketCommunication${Stage}"
    )
}

function Wait-Stack {
    param(
        [string] $condition
    )

    & aws @(
        'cloudformation'
        'wait'
        $condition
        '--stack-name'
        $StackName
    )
}

function New-Deployment {
    $apiId = Get-ExportValue -Name "WebSocketCommunication${Stage}RestApiGatewayId"

    & aws @(
        'apigateway'
        'create-deployment'
        '--rest-api-id'
        $apiId
        '--stage-name'
        $Stage
    )
}

Publish-Code
Publish-Template
Deploy-Stack
New-Deployment