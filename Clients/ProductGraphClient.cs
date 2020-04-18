using GraphQL;
using GraphQL.Client.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using UrbanCarton.Mvc.Models;

namespace UrbanCarton.Mvc.Clients
{
    public class ProductGraphClient
    {
        private readonly IGraphQLClient _graphQLClient;

        public ProductGraphClient(IGraphQLClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query productQuery($productId:ID!)
                {
                    product(id:$productID)
                    {
                       id name price rating photoFileName description stock introducedAt 
                      reviews { title review }
                    }
                }",
                Variables = new { productId = id }
            };

            var response = await _graphQLClient.SendQueryAsync<ProductModel>(query, CancellationToken.None);
            return response.Data;
        }

        public async Task AddReview(ProductReviewModel productReviewModel)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($review: reviewInput!)
                {
                    createReview(review: $review)
                        {
                            id
                        }
                }",
                Variables = new { productReviewModel }
            };

            var response = await _graphQLClient.SendMutationAsync<ProductReviewModel>(query);
            if (response.Errors is object)
            {
                // we got an error
            }
        }

        public void SubscribeToUpdates()
        {
            var query = new GraphQLRequest
            {
                Query = @"subscription { reviewAdded { title productId } }"
            };

            IObservable<GraphQLResponse<ProductReviewModel>> response =
                _graphQLClient.CreateSubscriptionStream<ProductReviewModel>(query);

            var subscription = response.Subscribe(response =>
            {
                Console.WriteLine(response.Data);
            });

            subscription.Dispose();
        }
    }
}
