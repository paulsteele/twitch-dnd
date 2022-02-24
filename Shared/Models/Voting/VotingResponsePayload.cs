using System;
using System.Collections.Generic;
using System.Linq;

namespace twitchDnd.Shared.Models.Voting;

public class VotingResponsePayload
{
	public List<VotingResponseModel> Responses { get; set; }
	public VotingResponseModel Winner { get; set; }

	public void CreateWinner()
	{
			var highestVoteNumber = Responses.Max(v => v.Votes);
			var matchingResponses = Responses.Where(v => v.Votes == highestVoteNumber);
			var random = new Random();
			var index = random.Next(matchingResponses.Count());
			Winner = Responses.Skip(index).First();
	}

	public static VotingResponsePayload DefaultPayload =>
		new VotingResponsePayload()
		{
			Responses = new List<VotingResponseModel>()
			{
				new VotingResponseModel() {Action = "Move"},
				new VotingResponseModel() {Action = "Talk"},
				new VotingResponseModel() {Action = "Hide"},
				new VotingResponseModel() {Action = "Attack"},
				new VotingResponseModel() {Action = "Defend"}
			}
		};
}