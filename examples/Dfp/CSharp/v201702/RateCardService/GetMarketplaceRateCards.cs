// Copyright 2016, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201702;
using Google.Api.Ads.Dfp.v201702;
using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201702 {
  /// <summary>
  /// This example gets all rate cards that can be used for Marketplace products.
  /// </summary>
  public class GetMarketplaceRateCards : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all rate cards that can be used for Marketplace products.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetMarketplaceRateCards codeExample = new GetMarketplaceRateCards();
      Console.WriteLine(codeExample.Description);
      try {
        codeExample.Run(new DfpUser());
      } catch (Exception e) {
        Console.WriteLine("Failed to get rate cards. Exception says \"{0}\"",
            e.Message);
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The DFP user object running the code example.</param>
    public void Run(DfpUser dfpUser) {
      RateCardService rateCardService =
          (RateCardService) dfpUser.GetService(DfpService.v201702.RateCardService);

      // Create a statement to select rate cards.
      int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("forMarketplace = :forMarketplace")
          .OrderBy("id ASC")
          .Limit(pageSize)
          .AddValue("forMarketplace", true);

      // Retrieve a small amount of rate cards at a time, paging through until all
      // rate cards have been retrieved.
      int totalResultSetSize = 0;
      do {
        RateCardPage page = rateCardService.getRateCardsByStatement(
            statementBuilder.ToStatement());

        // Print out some information for each rate card.
        if (page.results != null) {
          totalResultSetSize = page.totalResultSetSize;
          int i = page.startIndex;
          foreach (RateCard rateCard in page.results) {
            Console.WriteLine(
                "{0}) Rate card with ID {1}, name \"{2}\", and currency code \"{3}\" was found.",
                i++,
                rateCard.id,
                rateCard.name,
                rateCard.currencyCode
            );
          }
        }

        statementBuilder.IncreaseOffsetBy(pageSize);
      } while (statementBuilder.GetOffset() < totalResultSetSize);

      Console.WriteLine("Number of results found: {0}", totalResultSetSize);
    }
  }
}
