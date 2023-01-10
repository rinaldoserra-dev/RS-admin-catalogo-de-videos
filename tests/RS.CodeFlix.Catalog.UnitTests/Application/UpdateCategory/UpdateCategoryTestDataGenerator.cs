﻿using RS.CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;


namespace RS.CodeFlix.Catalog.UnitTests.Application.UpdateCategory
{
    public class UpdateCategoryTestDataGenerator
    {
        public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
        {
            var fixture = new UpdateCategoryTestFixture();

            for (int index = 0; index < times; index++)
            {
                var exampleCategory = fixture.GetExampleCategory();
                var exampleInput = fixture.GetValidInput(exampleCategory.Id);
                
                yield return new object[] {
                    exampleCategory,
                    exampleInput 
                };
            }
        }

        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdateCategoryTestFixture();
            var invalidInputList = new List<object[]>();
            var totalInvalidCases = 3;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputList.Add(new object[] {
                            fixture.GetInvalidInputShortName(),
                            "Name",
                            "Name should be at leats 3 characters long"
                        });
                        break;
                    case 1:
                        invalidInputList.Add(new object[] {
                            fixture.GetInvalidInputTooLongName(),
                            "Name",
                            "Name should be less or equal 255 characters long"
                        });
                        break;
                    case 2:
                        invalidInputList.Add(new object[] {
                            fixture.GetInvalidInputTooLongDescription(),
                            "Description",
                            "Description should be less or equal 10.000 characters long"
                        });
                        break;
                    default:
                        break;
                }
            }
            return invalidInputList;
        }
    }
}
