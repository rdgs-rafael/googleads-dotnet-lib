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
using Google.Api.Ads.Dfp.Util.v201611;
using Google.Api.Ads.Dfp.v201611;
using System;
using System.Collections.Generic;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201611 {
  /// <summary>
  /// This example gets all custom targeting values.
  /// </summary>
  public class GetAllCustomTargetingKeysAndValues : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all custom targeting values.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetAllCustomTargetingKeysAndValues codeExample = new GetAllCustomTargetingKeysAndValues();
      Console.WriteLine(codeExample.Description);
      try {
        codeExample.Run(new DfpUser());
      } catch (Exception e) {
        Console.WriteLine("Failed to get custom targeting values. Exception says \"{0}\"",
            e.Message);
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The DFP user object running the code example.</param>
    public void Run(DfpUser dfpUser) {
      CustomTargetingService customTargetingService =
          (CustomTargetingService) dfpUser.GetService(DfpService.v201611.CustomTargetingService);

      // Create a statement to select custom targeting values for a custom
      // targeting key.
      int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("customTargetingKeyId = :customTargetingKeyId")
          .OrderBy("id ASC")
          .Limit(pageSize);

      List<long> customTargetingKeyIds = getAllCustomTargetingKeyIds(dfpUser);

      // For each key, retrieve all its values.
      int totalValueCounter = 0;
      foreach (long customTargetingKeyId in customTargetingKeyIds) {
        // Set the custom targeting key ID to select from.
        statementBuilder.AddValue("customTargetingKeyId", customTargetingKeyId);

        // Retrieve a small amount of custom targeting values at a time, paging through until all
        // custom targeting values have been retrieved.
        int totalResultSetSize = 0;
        statementBuilder.Offset(0);
        do {
          CustomTargetingValuePage page =
              customTargetingService.getCustomTargetingValuesByStatement(
                  statementBuilder.ToStatement());

          // Print out some information for each custom targeting value.
          if (page.results != null) {
            totalResultSetSize = page.totalResultSetSize;
            foreach (CustomTargetingValue customTargetingValue in page.results) {
              Console.WriteLine(
                  "{0}) Custom targeting value with ID {1}, " +
                      "name \"{2}\", " +
                      "display name \"{3}\", " +
                      "and custom targeting key ID {4} was found.",
                  totalValueCounter++,
                  customTargetingValue.id,
                  customTargetingValue.name,
                  customTargetingValue.displayName,
                  customTargetingValue.customTargetingKeyId
              );
            }
          }

          statementBuilder.IncreaseOffsetBy(pageSize);
        } while (statementBuilder.GetOffset() < totalResultSetSize);
      }

      Console.WriteLine("Number of results found: {0}", totalValueCounter);
    }

    private List<long> getAllCustomTargetingKeyIds(DfpUser dfpUser) {
      List<long> customTargetingKeyIds = new List<long>();

      CustomTargetingService customTargetingService =
          (CustomTargetingService) dfpUser.GetService(DfpService.v201611.CustomTargetingService);

      // Create a statement to select custom targeting keys.
      int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
      StatementBuilder statementBuilder = new StatementBuilder()
          .OrderBy("id ASC")
          .Limit(pageSize);

      // Retrieve a small amount of custom targeting keys at a time, paging through until all
      // custom targeting keys have been retrieved.
      int totalResultSetSize = 0;
      do {
        CustomTargetingKeyPage page = customTargetingService.getCustomTargetingKeysByStatement(
            statementBuilder.ToStatement());

        // Print out some information for each custom targeting key.
        if (page.results != null) {
          totalResultSetSize = page.totalResultSetSize;
          int i = page.startIndex;
          foreach (CustomTargetingKey customTargetingKey in page.results) {
            Console.WriteLine(
                "{0}) Custom targeting key with ID {1}, " +
                    "name \"{2}\", " +
                    "and display name \"{3}\" was found.",
                i++,
                customTargetingKey.id,
                customTargetingKey.name,
                customTargetingKey.displayName
            );
            customTargetingKeyIds.Add(customTargetingKey.id);
          }
        }

        statementBuilder.IncreaseOffsetBy(pageSize);
      } while (statementBuilder.GetOffset() < totalResultSetSize);

      Console.WriteLine("Number of keys found: {0}", totalResultSetSize);

      return customTargetingKeyIds;
    }
  }
}
